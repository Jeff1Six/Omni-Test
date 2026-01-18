using Ambev.DeveloperEvaluation.ORM;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Unit.Helpers;

public static class DbContextHelper
{
    public static DefaultContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<DefaultContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new DefaultContext(options);
    }
}
