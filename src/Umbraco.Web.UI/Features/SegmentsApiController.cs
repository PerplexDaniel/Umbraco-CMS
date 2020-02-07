using System.Linq;
using System.Web.Http;
using Umbraco.Core.Models;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;
using Umbraco.Web.WebApi;

namespace Segments.Features.Segments
{
    public class SegmentsApiController : UmbracoApiController
    {
        private readonly IScopeProvider _scopeProvider;
        private readonly IContentService _contentService;
        private readonly IContentTypeService _contentTypeService;

        public SegmentsApiController(
            IScopeProvider scopeProvider,
            IContentService contentService,
            IContentTypeService contentTypeService)
        {
            _scopeProvider = scopeProvider;
            _contentService = contentService;
            _contentTypeService = contentTypeService;
        }

        [HttpGet]
        public void Setup()
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                var homepage = _contentService.GetByLevel(1)?.FirstOrDefault();
                if (homepage == null) return;

                var contentType = _contentTypeService.Get(homepage.ContentTypeId);

                // Add Segment variation type
                // contentType.Variations = contentType.Variations.SetFlag(ContentVariation.Segment, true);

                foreach (var propertyType in contentType.PropertyTypes)
                {
                    switch (propertyType.Alias)
                    {
                        case "propA":
                            propertyType.Variations = ContentVariation.CultureAndSegment;
                            break;

                        case "propB":
                            propertyType.Variations = ContentVariation.Culture;
                            break;

                        case "propC":
                            propertyType.Variations = ContentVariation.Segment;
                            break;

                        case "propD":
                            propertyType.Variations = ContentVariation.Nothing;
                            break;
                    }
                }

                _contentTypeService.Save(contentType);

                //homepage.SetValue("title", "Variant Title", culture: "en-US", segment: "b");

                //_contentService.SaveAndPublish(homepage);

                scope.Complete();
            }
        }
    }
}
