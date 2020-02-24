﻿namespace Umbraco.Core.Models.PublishedContent
{
    public static class VariationContextAccessorExtensions
    {
        public static void ContextualizeVariation(this IVariationContextAccessor variationContextAccessor, ContentVariation variations, ref string culture, ref string segment)
            => variationContextAccessor.ContextualizeVariation(variations, null, ref culture, ref segment);

        public static void ContextualizeVariation(this IVariationContextAccessor variationContextAccessor, ContentVariation variations, int contentId, ref string culture, ref string segment)
            => variationContextAccessor.ContextualizeVariation(variations, (int?)contentId, ref culture, ref segment);

        private static void ContextualizeVariation(this IVariationContextAccessor variationContextAccessor, ContentVariation variations, int? contentId, ref string culture, ref string segment)
        {
            if (culture != null && segment != null) return;

            // use context values
            var publishedVariationContext = contentId.HasValue
                ? variationContextAccessor?.GetVariationContext(contentId.Value)
                : variationContextAccessor?.VariationContext;

            if (culture == null) culture = variations.VariesByCulture() ? publishedVariationContext?.Culture : "";
            if (segment == null) segment = variations.VariesBySegment() ? publishedVariationContext?.Segment : "";
        }
    }
}
