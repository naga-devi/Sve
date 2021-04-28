using JxNet.Core;
using JxNet.Core.Extensions;
using JxNet.Extensions.ApiBase;
using JxNet.Extensions.WebHost.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Sve.Contract.Interface.Product;
using Sve.Contract.Models.Product;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using JxNet.Extensions.WebHost.Constants;
using Sve.Contract.ViewModels;
using JxNet.Core.Helpers;

namespace Sve.WebHost.Controllers
{
    [Authorize]
    [Route("api/product/productdetails")]
    [ApiController]
    public class ProductDetailsController : BaseController
    {
        private readonly IProductDetailsService _productDetailsService;
        private readonly IProductImagesService _imagesService;
        private readonly AppSettings _appSettings;

        public ProductDetailsController
        (
            AppSettings appSettings,
            IProductDetailsService productDetailsService,
            IProductImagesService imagesService
        )
        {
            _productDetailsService = productDetailsService;
            _imagesService = imagesService;
            _appSettings = appSettings;
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAllProducts()
        {
            var items = await _productDetailsService.GetAllProductsAsync();

            if (!items.HasItems())
                BadRequest("Invalid request");

            return Ok(items?.Select(x => new
            {
                x.Name,
                x.ProductId,
                x.CategoryId,
                x.TaxSlabId
            }).ToList());
        }

        [HttpPost]
        [Route("find")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GlobalSearch([FromBody] ProductFilterViewModel queryParameters)
        {
            var (totalCount, items) = await _productDetailsService.GetProductsAsync(queryParameters);

            return Ok(value: new
            {
                totalCount,
                items = items?.Select(x => new
                {
                    x.ProductId,
                    x.Name,
                    x.RatingsCount,
                    x.RatingsValue,
                    //Images = new List<Images> {
                    //    new Images {
                    //        Medium =$"{_appSettings.ApplicationUrl}/{x.ImagePath}"
                    //    },
                    //},
                    ImagePath = _appSettings.ApplicationUrl.FixUrl(x.ImagePath),
                    x.StockItems?.NewPrice,
                    x.StockItems?.Discount,
                    x.StockItems?.OldPrice,
                    x.AvailibilityCount,
                    //AvailibilityCount = x.StockedQuantity ?? 0 - x.SoldQuantity ?? 0,
                    x.CategoryName
                })
            });
        }

        [HttpGet]
        [Route("view/{id:int}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ViewProduct(int id)
        {
            var item = await _productDetailsService.GetByIdAsync(id);

            if (item == null)
                BadRequest("Invalid request");

            return Ok(value: new
            {
                id = item.ProductId,
                item.CategoryId,
                item.Name,
                item.RatingsCount,
                item.RatingsValue,
                item.Description,
                item.MinimumStock,
                item.Hsn,
                item.TaxSlabId,
                Images = item?.ProductImages?.Select(i => new
                {
                    //ImageId = i.ImageId,
                    Small = _appSettings.ApplicationUrl.FixUrl(i.Path),
                    Big = _appSettings.ApplicationUrl.FixUrl(i.Path),
                    Medium = _appSettings.ApplicationUrl.FixUrl(i.Path)
                }
                ).ToList()
            });
        }

        [HttpGet]
        [Route("id/{id:int}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProductDetailsById(int id)
        {
            var item = await _productDetailsService.GetByIdAsync(id);

            if (item == null)
                BadRequest("Invalid request");

            return Ok(value: new
            {
                id = item.ProductId,
                item.CategoryId,
                item.Name,
                item.RatingsCount,
                item.RatingsValue,
                item.Description,
                item.MinimumStock,
                item.Hsn,
                item.TaxSlabId,
                Images = item?.ProductImages?.Select(i => new
                {
                    i.ImageId,
                    //Small = $"{_appSettings.ApplicationUrl}/{i.Path}",
                    //Big = $"{_appSettings.ApplicationUrl}/{i.Path}",
                    Medium = _appSettings.ApplicationUrl.FixUrl(i.Path)
                }
                ).ToList()
            });
        }

        [HttpPost]
        [Route("save")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> SaveProductDetails([FromBody] ProductCreateRequestModel entity)
        {

            if (entity == null)
                return BadRequest("Invalid request.");

            var product = new ProductDetails()
            {
                TaxSlabId = entity.TaxSlabId ?? 1, // default tax 18
                ProductId = entity.ProductId,
                CategoryId = entity.CategoryId,
                Name = entity.Name,
                Description = entity.Description,
                MinimumStock = entity.MinimumStock,
                Hsn = entity.Hsn,
                RatingsCount = entity.RatingsCount,
                RatingsValue = entity.RatingsValue,
                Status = 1
            };

            var result = await _productDetailsService.SaveAsync(product);

            if (result.Status == Status.Success)
            {
                if (entity.AddedImages.HasItems())
                {
                    var images = new List<ProductImages>();
                    entity.AddedImages.ForEach(x =>
                    {
                        var imagePath = LoadImage(x.Base64Data, x.Name);
                        images.Add(new ProductImages
                        {
                            ProductId = (int?)result.NewId,
                            Path = imagePath,
                            Status = 1,
                        });
                    });

                    var imageResponse = await _imagesService.CreateAsync(images);

                    if (imageResponse.Status == Status.Error)
                    {
                        result.Message = "Product updated successfully bt images are not added. Please try again.";
                        DeleteImagesFromPhysycalLocation(images);
                    }
                }

                if (entity.DeletedImages.HasItems())
                {
                    var (response, items) = await _imagesService.DeleteByIdAsync(entity.DeletedImages?.ToArray());
                    if (response.Status == Status.Success)
                    {
                        DeleteImagesFromPhysycalLocation(items);
                    }
                }
            }

            return Ok(result);
        }

        [HttpDelete]
        [Route("delete/{productIds}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteProductDetails(string productIds)
        {
            (ResponseResult response, List<ProductImages> images) = await _productDetailsService.DeleteByIdAsync(productIds.ToNullableIntegerArray());

            if (response.Status == Status.Success && images.HasItems())
            {
                DeleteImagesFromPhysycalLocation(images);
            }

            return Ok(response);
        }

        //[NonAction]
        //[HttpPost]
        //public IActionResult Upload(IFormFile file)
        //{
        //    //http://www.ziyad.info/en/articles/29-Image_Resize_for_NetCore // alternative

        //    using (var image = Image.Load(file.OpenReadStream()))
        //    {
        //        image.Mutate(x => x.Resize(256, 256));
        //        image.Save($"wwwroot\\uploads\\products\\{file.FileName}");
        //    }
        //    return Ok();
        //}

        private string LoadImage(string data, string fileName)
        {
            var fileExtension = fileName.GetFileExtension();
            var folderPath = ImageConstants.ProductUploadPath;

            if (!System.IO.Directory.Exists(System.IO.Path.Combine(_appSettings.WebRootPath, folderPath)))
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(_appSettings.WebRootPath, folderPath));
            }

            var fileSavePath = $"{folderPath}\\{Guid.NewGuid()}.{fileExtension}";
            //data:image/gif;base64,
            //this image is a single pixel (black)
            var base64Content = Base64ImageHelper.Parse(data);

            using (var image = Image.Load(base64Content.FileContents))
            {
                var width = image.Width > ImageConstants.Medium.Width ? ImageConstants.Medium.Width : image.Width;
                var height = image.Height > ImageConstants.Medium.Height ? ImageConstants.Medium.Height : image.Height;

                if (image.Width < image.Height || image.Width > ImageConstants.Medium.Width || image.Height > ImageConstants.Medium.Height)
                {
                    image.Mutate(x => x.Resize(width, height));//(217*168)// intrinsic:480*360
                }


                //////Size can be change according to your requirement 
                ////float thumbWidth = ImageConstants.Medium.Width;
                ////float thumbHeight = ImageConstants.Medium.Height;
                //////calculate  image  size
                ////if (image.Width > image.Height)
                ////{
                ////    thumbHeight = ((float)image.Height / image.Width) * thumbWidth;
                ////}
                ////else
                ////{
                ////    thumbWidth = ((float)image.Width / image.Height) * thumbHeight;
                ////}

                ////int actualthumbWidth = Convert.ToInt32(Math.Floor(thumbWidth));
                ////int actualthumbHeight = Convert.ToInt32(Math.Floor(thumbHeight));


                image.Save(_appSettings.WebRootPath.CombinePath(fileSavePath));

                return fileSavePath;
            }
        }

        private void DeleteImagesFromPhysycalLocation(List<ProductImages> items)
        {
            if (items.HasItems())
            {
                items.ForEach(async x =>
                {
                    await Task.Run(() =>
                    {
                        FileHelper.Delete(_appSettings.WebRootPath.CombinePath(x.Path));
                    });
                });
            }
        }
    }
}