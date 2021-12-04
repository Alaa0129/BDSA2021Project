using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp.Core
{
    public interface ITagRepository
    {
        Task <IReadOnlyCollection<TagDetailsDTO>> ReadAsyncAll();
    }
}