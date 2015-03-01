using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace USBTetminal2.Utils
{
    //NOT USED FOR NOW
    public class CustomRegionAdapter : RegionAdapterBase<ContentControl>
    {
        public CustomRegionAdapter(IRegionBehaviorFactory behaviorFactory)
            : base(behaviorFactory)
        {
        }

        protected override void Adapt(IRegion region, ContentControl regionTarget)
        {
            bool contentIsSet = regionTarget.Content != null;
            if (contentIsSet)
                throw new InvalidOperationException("Your custom exception message");

            region.ActiveViews.CollectionChanged += delegate
            {
                regionTarget.Content = region.ActiveViews.FirstOrDefault();
            };

            region.Views.CollectionChanged +=
                (sender, e) =>
                {
                    if (e.Action == NotifyCollectionChangedAction.Add && region.ActiveViews.Count() == 0)
                    {
                        region.Activate(e.NewItems[0]);
                    }
                };
        }


        protected override IRegion CreateRegion()
        {
            return new SingleActiveRegion();
        }
    }
}
