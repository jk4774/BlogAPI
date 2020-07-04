using System;
using System.ComponentModel.DataAnnotations;

namespace BlogEntities
{
    public class Password
    {
        public string Old { get; set; }
        public string New { get; set; }
    }
}