using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Models.Response.Basis;

namespace AquariumManagementAPI.Controllers
{
    public static class ActionResultExtension
    {
        public static ActionResult<ItemResponseModel<T>> ToResponse<T>(this Task<ItemResponseModel<T>> result) where T : Entity
        {
            ActionResult<ItemResponseModel<T>> tsk = HandleResult<T>(result);
            return tsk;
        }


        public static ActionResult<ItemResponseModel<T>> ToResponse<T>(this ItemResponseModel<T> result) where T : Entity
        {
            ActionResult<ItemResponseModel<T>> tsk = HandleResult<T>(result);
            return tsk;
        }

        private static ActionResult<ItemResponseModel<T>> HandleResult<T>(Task<ItemResponseModel<T>> result) where T : Entity
        {
            if (result == null)
            {
                return new BadRequestResult();
            }
            if (result.Result == null)
            {
                return new BadRequestObjectResult(result);
            }
            else if (result.Result.HasError == false)
            {
                OkObjectResult obj = new OkObjectResult(result.Result);
                obj.Value = result;

                return obj;
            }
            else
            {
                if (result.Result.ErrorMessages.ContainsKey("NotFound"))
                {
                    return new NotFoundObjectResult(result.Result);
                }
                else
                {
                    return new BadRequestObjectResult(result.Result);
                }
            }

        }


        private static ActionResult<ItemResponseModel<T>> HandleResult<T>(ItemResponseModel<T> result) where T : Entity
        {
            if (result == null)
            {
                return new BadRequestResult();
            }

            else if (result.HasError == false)
            {
                OkObjectResult obj = new OkObjectResult(result);
                obj.Value = result;

                return obj;
            }
            else
            {
                if (result.ErrorMessages.ContainsKey("NotFound"))
                {
                    return new NotFoundObjectResult(result);
                }
                else
                {
                    return new BadRequestObjectResult(result);
                }
            }

        }

    }
}
