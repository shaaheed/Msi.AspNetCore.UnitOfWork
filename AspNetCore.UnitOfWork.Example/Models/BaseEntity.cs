using System;

namespace AspNetCore.UnitOfWork.Example.Models
{
    public class BaseEntity : IEntity
    {

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
