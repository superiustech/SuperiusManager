using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ApplicationDbContextMaster _dbContextMaster;
        public ClienteRepository(ApplicationDbContextMaster dbContext)
        {
            _dbContextMaster = dbContext;
        }
        public async Task<CWCliente> CadastrarCliente(CWCliente oCWCliente)
        {
            using (var transaction = await _dbContextMaster.Database.BeginTransactionAsync())
            {
                try
                {
                    var clienteExistente = await _dbContextMaster.Cliente.FirstOrDefaultAsync(x => x.nCdCliente == oCWCliente.nCdCliente || x.sNmCliente == oCWCliente.sNmCliente);

                    if (clienteExistente == null)
                    {
                        await _dbContextMaster.AddAsync(oCWCliente);
                        await _dbContextMaster.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return oCWCliente;
                    }
                    else
                    {
                        clienteExistente.sNmCliente = oCWCliente.sNmCliente;
                        clienteExistente.CNPJ = oCWCliente.CNPJ;
                        clienteExistente.bFlAtivo = oCWCliente.bFlAtivo;

                        _dbContextMaster.Entry(clienteExistente).State = EntityState.Modified;

                        await _dbContextMaster.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return clienteExistente;
                    }
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

    }
}


