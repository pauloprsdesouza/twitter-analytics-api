namespace Twitter.Analytics.Infrastructure.Database.DataModel.BaseModels
{
    public class BaseKey<T> where T : BaseModel
    {
        public string PK { get; set; }
        public string SK { get; set; }
        public string GSIPK { get; set; }
        public string GSISK { get; set; }

        public void AssignTo(T entityModel)
        {
            entityModel.PK = PK;
            entityModel.SK = SK;
            entityModel.GSISK = GSISK;
            entityModel.GSIPK = GSIPK;
        }
    }
}
