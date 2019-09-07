using System;

namespace AspNetCore.UnitOfWork.Example.Models
{
    public class Todo : BaseEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Done { get; set; }

    }
}
