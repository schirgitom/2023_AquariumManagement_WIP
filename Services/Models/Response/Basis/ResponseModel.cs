namespace Services.Models.Response.Basis
{
    public class ResponseModel
    {
        public bool HasError { get; set; }
        public Dictionary<string, string> ErrorMessages { get; set; } = new();

        public Dictionary<string, string> WarningMessages { get; set; } = new();

        public bool HasWarning
        {
            get
            {
                if (WarningMessages != null && WarningMessages.Count > 0)
                {
                    return true;
                }

                return false;
            }
        }

        public static ResponseModel CreateResponseError(string errormessage)
        {
            ResponseModel resp = new ResponseModel();
            resp.HasError = true;
            resp.ErrorMessages.Add("Error", errormessage);

            return resp;
        }


        public static ResponseModel CreateResponseError(string errormessage, string errorkey)
        {
            ResponseModel resp = new ResponseModel();
            resp.HasError = true;
            resp.ErrorMessages.Add(errorkey, errormessage);

            return resp;
        }

        public void AddWarningMessageRange(Dictionary<string, string> input)
        {
            foreach (KeyValuePair<string, string> inp in input)
            {
                if (WarningMessages.ContainsKey(inp.Key) == false)
                {
                    WarningMessages.Add(inp.Key, inp.Value);
                }
            }
        }

        public void AddErrorMessageRange(Dictionary<string, string> input)
        {
            foreach (KeyValuePair<string, string> inp in input)
            {
                if (ErrorMessages.ContainsKey(inp.Key) == false)
                {
                    ErrorMessages.Add(inp.Key, inp.Value);
                }
            }
        }
    }
}
