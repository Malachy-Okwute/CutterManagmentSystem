namespace CutterManagement.Core
{
    public static class DataModelDtoMapper
    {
        public static T Map<T>(this object source) where T : new()
        {
            if(source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var target = new T();
            var targetProperties = typeof(T).GetProperties();
            var sourceProperties = source.GetType().GetProperties();

            sourceProperties.ToList().ForEach(property =>
            {
                var targetProperty = targetProperties.FirstOrDefault(p => p.Name.Equals(property.Name) && p.PropertyType.Equals(property.PropertyType));

                if(targetProperty is not null && targetProperty.CanWrite)
                {
                    targetProperty.SetValue(target, property.GetValue(source));
                }
            });

            return target;
        }
    }
}
