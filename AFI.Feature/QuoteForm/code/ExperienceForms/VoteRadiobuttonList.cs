using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Linq;

namespace AFI.Feature.QuoteForm.ExperienceForms
{
    public class VoteRadiobuttonList : Sitecore.Web.UI.HtmlControls.Control
    {
        // Property to hold the parent ID
        public string ParentId { get; set; }

        // Method to get all child items based on the parent ID
        private List<Item> GetAllChildren()
        {
            if (string.IsNullOrEmpty(ParentId))
            {
                return new List<Item>();
            }

            var parentItem = Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(ParentId));
            if (parentItem == null)
            {
                return new List<Item>();
            }

            return parentItem.Children.ToList();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Render the radio button list
            RenderRadioButtonList();
        }

        private void RenderRadioButtonList()
        {
            Controls.Clear();

            // Get all child items
            var childItems = GetAllChildren();

            if (childItems.Count > 0)
            {
                var radioButtonGroup = new Sitecore.Web.UI.HtmlControls.Radiogroup();
                radioButtonGroup.ID = "rbGroup";

                // Render each child item as a radio button
                foreach (var childItem in childItems)
                {
                    var radioButton = new Sitecore.Web.UI.HtmlControls.Radiobutton();
                    radioButton.ID = "rb_" + childItem.ID.ToShortID();
                    radioButton.Value = "";
                    radioButton.Name = childItem.DisplayName;
                    radioButtonGroup.Controls.Add(radioButton);

                    var label = new System.Web.UI.HtmlControls.HtmlGenericControl("label");
                    label.Attributes["for"] = "rb_" + childItem.ID.ToShortID();
                    label.InnerText = childItem.DisplayName;
                    radioButtonGroup.Controls.Add(label);

                    radioButtonGroup.Controls.Add(new System.Web.UI.HtmlControls.HtmlGenericControl("br"));
                }

                Controls.Add(radioButtonGroup);
            }
        }
    }
}