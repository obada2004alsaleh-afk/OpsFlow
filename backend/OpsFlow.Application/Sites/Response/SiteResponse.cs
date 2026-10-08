using System;
using System.Collections.Generic;
using System.Text;

namespace OpsFlow.Application.Sites.Response
{
    public class SiteResponse
    {
        public int SiteId { get; set; }
        public required string Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public required string Address { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
