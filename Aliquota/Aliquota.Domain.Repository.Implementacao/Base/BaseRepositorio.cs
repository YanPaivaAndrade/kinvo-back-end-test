﻿using Aliquota.Domain.Entities.Interface;
using Aliquota.Domain.Repository.Implementacao.Context;
using Aliquota.Domain.Repository.IRepositorios;
using Aliquota.Domin.Util.Excecoes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aliquota.Domain.Repository.Implementacao.Base
{
    public class BaseRepositorio<T> : IBaseRepositorio<T>, IDisposable where T : class, IEntidade<int>
    {

        protected AliquotaDbContext _context;
        private bool _disposed = false;
        public BaseRepositorio(AliquotaDbContext context)
        {
            _context = context;
        }

        public async Task<Task> Add(T entidade)
        {
            try
            {
                await _context.Set<T>().AddAsync(entidade);
                await _context.SaveChangesAsync();
                return Task.CompletedTask;
            }
            catch (DbUpdateException)
            {
                throw new IntegridadeException("Não foi possível completar a inserção da entidade "+ entidade.GetType() + " "+ entidade.Descricao);
            }
        }

        public Task AddAll(IEnumerable<T> entidadeLista)
        {
            try
            {
                _context.Set<T>().AddRange(entidadeLista);
                _context.SaveChanges();
                return Task.CompletedTask;
            }
            catch (DbUpdateException)
            {
                throw new IntegridadeException("Não foi possível completar a inserção da entidade " + entidadeLista.GetType() + " " + entidadeLista.First().Descricao);
            }
        }

        public async Task<Task> AddAllAsync(IEnumerable<T> entidadeLista)
        {
            try
            {
                await _context.Set<T>().AddRangeAsync(entidadeLista);
                await _context.SaveChangesAsync();
                return Task.CompletedTask;
            }
            catch (DbUpdateException)
            {
                throw new IntegridadeException("Não foi possível completar a inserção da entidade " + entidadeLista.GetType() + " " + entidadeLista.First().Descricao);
            }
        }

        public async Task<IEnumerable<T>> AllAssync()
        {
            var entidadeLista = await _context.Set<T>().ToListAsync();
            try
            {
                if (!entidadeLista.Any())
                {
                    throw new NaoEncontradoException(@"Objetos não encontrado no banco de dados! ");
                }

                return entidadeLista;
            }
            catch (DbUpdateException)
            {
                throw new IntegridadeException("Não foi possível completar a inserção da entidade " + entidadeLista.GetType() + " " + entidadeLista.First().Descricao);
            }
        }

        public Task Delete(T Entidade)
        {
            try
            {
                _context.Set<T>().Remove(Entidade);
                _context.SaveChangesAsync();
                return Task.CompletedTask;
            }
            catch (DbUpdateException)
            {
                throw new IntegridadeException("Não foi possível completar a deleção da entidade " + Entidade.GetType() + " " + Entidade.Descricao);
            }
        }

        public Task Delete(object id)
        {

            var obj = _context.Set<T>().FirstOrDefault(a => a.Id == (int)id);
            try
            {
                if(obj == null)
                {
                    throw new NaoEncontradoException(@"Objeto não encontrado no banco de dados! ");
                }

                _context.Set<T>().Remove(obj);
                _context.SaveChanges();
                return Task.CompletedTask;
            }
            catch (DbUpdateException)
            {
                throw new IntegridadeException("Não foi possível completar a deleção da entidade " + obj.GetType() + " " + obj.Descricao);
            }
            
        }

        public Task DeleteAll(IEnumerable<T> entidadeLista)
        {
            try
            {
                _context.Set<T>().RemoveRange(entidadeLista);
                _context.SaveChanges();
                return Task.CompletedTask;
            }
            catch (DbUpdateException)
            {
                throw new IntegridadeException("Não foi possível completar as deleções das entidades " + entidadeLista.GetType() + " " + entidadeLista.First().Descricao);
            }
        }

        public async Task<Task> DeleteAllAsync(IEnumerable<T> entidadeLista)
        {
            try
            {
                _context.Set<T>().RemoveRange(entidadeLista);
                await _context.SaveChangesAsync();
                return Task.CompletedTask;
            }
            catch (DbUpdateException)
            {
                throw new IntegridadeException("Não foi possível completar as deleções das entidades " + entidadeLista.GetType() + " " + entidadeLista.First().Descricao);
            }
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetById(object id)
        {
            var entidade = await _context.Set<T>().FirstOrDefaultAsync(a => a.Id == (int)id);
            try
            {
                if(entidade == null)
                {
                    throw new NaoEncontradoException(@"Objeto não encontrado no banco de dados! ");
                }

                return entidade;
            }
            catch (DbUpdateException)
            {
                throw new IntegridadeException("Não foi possível completar a busca da entidade " + entidade.GetType() + " " + entidade.Descricao);
            }
        }

        public IQueryable<T> GetQueryable()
        {   
            return _context.Set<T>().AsQueryable();
        }

        public Task Update(T entidade)
        {
            try
            {
                _context.Set<T>().Update(entidade);
                _context.SaveChanges();
                return Task.CompletedTask;
            }
            catch (DbUpdateException)
            {
                throw new IntegridadeException("Não foi possível completar a atualização da entidade " + entidade.GetType() + " " + entidade.Descricao);
            }
        }

        public async Task<Task> UpdateAll(IEnumerable<T> entidadeLista)
        {
            try
            {
                 _context.Set<T>().UpdateRange(entidadeLista);
                await _context.SaveChangesAsync();
                return Task.CompletedTask;
            }
            catch (DbUpdateException)
            {
                throw new IntegridadeException("Não foi possível completar a atualização das entidades " + entidadeLista.GetType() + " " + entidadeLista.First().Descricao);
            }
        }

    }
}