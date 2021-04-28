namespace Sve.Service.Data
{
    public interface IDbInitializer
    {
        void Migrate();
        void Seed();
    }
}
