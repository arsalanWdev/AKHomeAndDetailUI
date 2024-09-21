    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace AK.Models
    {
        public class Portfolio
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string ImageUrl { get; set; }

            // Foreign Key to User
            public string UserId { get; set; }
            public virtual ApplicationUser User { get; set; }
        }

    }
