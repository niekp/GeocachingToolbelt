using System;
namespace GeocachingToolbelt.Models
{
    public class Tool
    {
        public Tool(string Title, string Description, string Image, string Controller, string Action = "Index")
        {
            this.Title = Title;
            this.Description = Description;
            this.Image = Image;
            this.Controller = Controller;
            this.Action = Action;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
    }
}
