namespace HomeCinema.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private HomeCinemaContext _dbContext;

        public HomeCinemaContext Init()
        {
            return _dbContext ?? (_dbContext = new HomeCinemaContext());
        }

        protected override void DisposeCore()
        {
            if (_dbContext != null)
                _dbContext.Dispose();
        }
    }
}