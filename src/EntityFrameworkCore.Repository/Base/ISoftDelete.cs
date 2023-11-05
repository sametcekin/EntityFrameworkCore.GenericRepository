namespace EntityFrameworkCore.GenericRepository.Base
{
    public interface ISoftDelete
    {
        public bool IsDeleted { get; set; }
    }
}