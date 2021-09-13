﻿using FletcherProj.Dtos;
using FletcherProj.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace FletcherProj.Commons
{
    public class Utilities
    {
        public static PageMetadata Paginate(int page, int per_page, int total)
        {
            int total_page = total % per_page == 0 ? total / per_page : total / per_page + 1;
            var result = new PageMetadata()
            {
                Page = page,
                PerPage = per_page,
                Total = total,
                TotalPages = total_page
            };
            return result;
        }

        public static ResponseDto<T> CreateResponse<T>(string message,
            ModelStateDictionary errs, T data)
        {
            var errors = new Dictionary<string, string>();
            if (errs != null)
            {
                foreach (var err in errs)
                {
                    var counter = 0;
                    var key = err.Key;
                    var errVals = err.Value;
                    foreach (var errMsg in errVals.Errors)
                    {
                        errors.Add($"{(counter + 1)} - {key}", errMsg.ErrorMessage);
                        counter++;
                    }
                }
            }

            var obj = new ResponseDto<T>()
            {
                Message = message,
                Errs = errors,
                Data = data
            };
            return obj;
        }
    }
}
