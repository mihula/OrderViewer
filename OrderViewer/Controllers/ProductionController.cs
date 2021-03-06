﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderViewer.Core.DataLayer.Contracts;
using OrderViewer.Core.DataLayer.DataContracts;
using OrderViewer.Extensions;
using OrderViewer.Responses;

namespace OrderViewer.Controllers
{
    [Route("api/[controller]")]
    public class ProductionController : Controller
    {
        private IProductionRepository ProductionRepository;

        public ProductionController(IProductionRepository repository)
        {
            ProductionRepository = repository;
        }

        protected override void Dispose(Boolean disposing)
        {
            ProductionRepository?.Dispose();

            base.Dispose(disposing);
        }

        [HttpGet("ProductSubcategory")]
        public async Task<IActionResult> GetProductSubcategories(Int32? pageSize = 10, Int32? pageNumber = 1)
        {
            var response = new ListModelResponse<ProductSubcategoryViewModel>() as IListModelResponse<ProductSubcategoryViewModel>;

            try
            {
                response.PageSize = (Int32)pageSize;
                response.PageNumber = (Int32)pageNumber;

                response.Model = await Task.Run(() =>
                {
                    return ProductionRepository
                        .GetProductSubcategories((Int32)pageSize, (Int32)pageNumber);
                });

                response.Message = String.Format("Total of records: {0}", response.Model.Count());
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message;
            }

            return response.ToHttpResponse();
        }
    }
}
