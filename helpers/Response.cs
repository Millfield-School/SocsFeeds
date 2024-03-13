using System;
using System.Collections.Generic;
using System.Text;

namespace SocsFeeds.helpers
{
    public class Response<T>
    {
        // Can be Used  Different ways when returning the data from methods
        //
        // 1.
        //      return Response<Root>.Success(JsonConvert.DeserializeObject<Root>(jsonString));
        //      return Response<Root>.Error(errorMessage);
        //      return Response<Root>.Success(new Root {    visits = totalVisits, totalCount = totalVisits?.Count ?? 0, count = totalVisits?.Count ?? 0, });
        //
        // 2.
        //      return new Response<bool> { ErrorMessage = errorMessage, Data = false};
        //      return new Response<bool> { Data = true };
        //             
        // 1st where you return a success or true with ONLY returning data or error (see methods bellow)
        // 2nd allows you to return data and error if you wish 

        public T Data { get; set; }
        public string ErrorMessage { get; set; }

        public static Response<T> Success(T data)
        {
            return new Response<T>() { Data = data };
        }

        public static Response<T> Error(string errorMessage)
        {
            return new Response<T>() { ErrorMessage = errorMessage };
        }
    }
}
