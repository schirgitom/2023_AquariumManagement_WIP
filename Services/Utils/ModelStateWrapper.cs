using Microsoft.AspNetCore.Mvc.ModelBinding;
using Utils;

namespace Services.Utils
{
    public class ModelStateWrapper : IModelStateWrapper
    {
        private ModelStateDictionary _modelState;

        protected Serilog.ILogger log = Logger.ContextLog<ModelStateWrapper>();

        public ModelStateWrapper(ModelStateDictionary modelState)
        {
            _modelState = modelState;
            Clear();
        }

        public void AddError(string key, string errorMessage)
        {
            log.Debug("Model has an Error: {Error} - Message: {Message}", key, errorMessage);
            _modelState.AddModelError(key, errorMessage);
        }

        public bool IsValid
        {
            get { return _modelState.IsValid; }
        }

        public void Clear()
        {
            _modelState.Clear();
        }

        public Dictionary<string, string> Errors
        {

            get
            {
                Dictionary<String, String> errors = new Dictionary<string, string>();

                foreach (KeyValuePair<String, ModelStateEntry> err in _modelState)
                {
                    ModelStateEntry modelStateEntry = err.Value;

                    String errormessage = "";

                    ModelErrorCollection coll = modelStateEntry.Errors;

                    foreach (ModelError error in coll)
                    {
                        errormessage += error.ErrorMessage;
                    }


                    errors.Add(err.Key, errormessage);
                }

                return errors;

            }
        }
    }
}
