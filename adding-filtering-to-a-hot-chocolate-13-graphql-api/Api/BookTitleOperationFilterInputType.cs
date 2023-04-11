using HotChocolate.Data.Filters;

namespace Api;

public class BookTitleOperationFilterInputType : StringOperationFilterInputType
{
    protected override void Configure(IFilterInputTypeDescriptor descriptor)
    {
        descriptor.Operation(DefaultFilterOperations.Equals).Type<StringType>();
    }
}
