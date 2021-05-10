using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Plutus.Application.Categories.ViewModels;
using Plutus.Application.Repositories;

namespace Plutus.Application.Categories.Queries
{
    public static class FindCategories
    {
        public record Request: IRequest<IEnumerable<CategoryViewModel>>;
        
        public class Handler: IRequestHandler<Request, IEnumerable<CategoryViewModel>>
        {
            private readonly ICategoryRepository _repository;
            private readonly IMapper _mapper;

            public Handler(ICategoryRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            
            public async Task<IEnumerable<CategoryViewModel>> Handle(Request request, CancellationToken cancellationToken)
            {
                var categories = await _repository.FindAsync();
                return _mapper.Map<IEnumerable<CategoryViewModel>>(categories);
            }
        }
    }
}