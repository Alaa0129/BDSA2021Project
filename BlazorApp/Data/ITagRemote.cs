using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorApp.Core;

namespace BlazorApp
{
    public interface ITagRemote
    {
        Task<IEnumerable<TagDetailsDTO>> GetTags();

    }
}