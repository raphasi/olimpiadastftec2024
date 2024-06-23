using CRM.Domain.Entities;
using CRM.WebApp.Site.Models;
using Microsoft.AspNetCore.Mvc;

public abstract class BaseController<T> : Controller where T : EntityBase, new()
{
    protected T InitializeEntity()
    {
        return new T
        {
            CreatedOn = DateTime.Now,
            CreatedBy = Guid.NewGuid(), // ou obtenha o ID do usuário logado
            ModifiedOn = DateTime.Now,
            ModifiedBy = Guid.NewGuid(), // ou obtenha o ID do usuário logado
            IsNew = true
        };
    }

    protected void UpdateEntity(T entity)
    {
        entity.ModifiedOn = DateTime.Now;
        entity.ModifiedBy = Guid.NewGuid(); // ou obtenha o ID do usuário logado
        entity.IsNew = false;
    }
}