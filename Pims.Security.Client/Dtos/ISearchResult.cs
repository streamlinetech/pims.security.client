using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlitBit.Dto;

namespace Streamline.Pims.Security.Client.Dtos
{
    [DTO]
    public interface ISearchResult
    {
        int Count { get; set; }
        int PageSize { get; set; }
        int TotalPages { get; set; }
        int PageIndex { get; set; }
    }
}
