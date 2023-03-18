using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace MoviesApi.Helpers
{
    public class TypeBinder<T> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var propName = bindingContext.ModelName;
            var valueProvider = bindingContext.ValueProvider.GetValue(propName);

            if( valueProvider == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            try
            {
                var deserialize = JsonConvert.DeserializeObject<List<T>>(valueProvider.FirstValue);
                bindingContext.Result = ModelBindingResult.Success(deserialize);
            }catch
            {
                bindingContext.ModelState.TryAddModelError(propName, " Invalid value for type List<int> ");
            }

            return Task.CompletedTask;
        }
    }
}
