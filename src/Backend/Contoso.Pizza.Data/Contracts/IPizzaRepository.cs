using DM = Contoso.Pizza.Data.Models;

namespace Contoso.Pizza.Data.Contracts;

public interface IPizzaRepository : IRepository<DM.Pizza>
{ }
