namespace Sve.Service
{
    internal class CommonConstants
    {
        public const string ActionCommand_SaveSuccessMessage = "New {0} has been added successfully.";
        public const string ActionCommand_EntityAlreadyExistMessage = "{0} existed already.";
        public const string ActionCommand_SaveErrorMessage = "{0} not saved. Error in processing. Please contact administrator.";
        public const string ActionCommand_UpdateSuccessMessage = "{0} updated successfully.";
        public const string ActionCommand_UpdateErrorMessage = "{0} not updated. Error in processing. Please contact administrator.";
        public const string ActionCommand_DeleteSuccessMessage = "{0} deleted successfully.";
        public const string ActionCommand_DeleteErrorMessage = "{0} not deleted. Error in processing. Please contact administrator.";
        public const string ActionCommand_StatusSuccessMessage = "{0} status changed successfully.";
        public const string ActionCommand_StatusErrorMessage = "{0} not changed. Error in processing. Please contact administrator.";
    }

    public enum CacheKeys : byte
    {
        ProductCategoryCacheKey,
        TaxSlabs
    }
}
