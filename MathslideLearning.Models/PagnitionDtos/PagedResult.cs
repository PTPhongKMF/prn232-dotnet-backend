using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathslideLearning.Models.PagnitionDtos
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Results { get; set; } = new List<T>();
        public PaginationDto Pagnition { get; set; } = new();
    }
}
