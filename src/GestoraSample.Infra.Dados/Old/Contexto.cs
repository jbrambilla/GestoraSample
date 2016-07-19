using Microsoft.Win32;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml;

namespace GestoraSample.Infra.Dados.Old
{
    
    public class Contexto : IDisposable
    {
        protected SqlCommand Command { get; set; }
        private SqlConnection Connection;
        protected static SqlTransaction Transaction;

        /// <summary>
        /// Define a string de conexão lendo o arquivo XML
        /// </summary>
        public Contexto()
        {
            string Caminho = @"Data Source=192.168.0.66;"
                             + "User ID = evandro; Password = mudama23;"
                             + "Initial Catalog=DB_GESTORASAMPLE;"
                             + "Integrated Security=SSPI;"
                             + "Encrypt =False; TrustServerCertificate=False;";

            Connection = new SqlConnection(Caminho);
        }

        /// <summary>
        /// Define a conexão recebendo a string de conexão por parâmetro.
        /// </summary>
        /// <param name="stringConexao"></param>
        public Contexto(string stringConexao)
        {
            Connection = new SqlConnection(stringConexao);
        }

        /// <summary>
        /// Método para abrir a conexão com o banco de dados
        /// </summary>
        /// <param name="isTransaction"></param>
        /// <returns></returns>
        protected bool AbreConexao(bool isTransaction)
        {
            try
            {
                if (Connection.State == ConnectionState.Closed)
                    Connection.Open();
                if (isTransaction)
                {
                    if (Transaction == null || Transaction.Connection == null)
                        Transaction = Connection.BeginTransaction();
                    Command.Transaction = Transaction;
                    Command.Connection = Transaction.Connection;
                }
                else
                    Command.Connection = Connection;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Fecha conxão com o banco.
        /// </summary>
        /// <returns></returns>
        protected bool FechaConexao()
        {
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Finaliza transação.
        /// </summary>
        /// <param name="isCommit"></param>
        public void FinalizarTransacao(bool isCommit)
        {
            if (Transaction.Connection != null && Transaction.Connection.State == ConnectionState.Open)
            {
                if (isCommit)
                    Transaction.Commit();
                else
                    Transaction.Rollback();
                FechaConexao();
            }
        }

        /// <summary>
        /// Responsável pela execução dos comandos de Insert, Update e Delete
        /// </summary>
        /// <returns>Retorna um número inteiro que indica a quantidade de linhas afetadas</returns>
        public int ExecutaComando(bool isTransacao)
        {
            int retorno;
            AbreConexao(isTransacao);
            try
            {
                retorno = Command.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                if (exception.GetType() == typeof(SqlException))
                {
                    SqlException sqlException = (SqlException)exception;
                    retorno = sqlException.Errors[0].Number * -1;
                }
                else
                {
                    retorno = -1;
                }
#if DEBUG
                throw new Exception("Erro ao executar o comando SQL:", exception);
#endif
            }
            finally
            {
                if (!isTransacao)
                    FechaConexao();
            }
            return retorno;
        }

        /// <summary>
        /// Executa insert e retorna o último código cadastrado
        /// </summary>
        /// <returns>Quantidade de linhas afetadas</returns>
        protected int ExecutaComandoScalar(bool transacao)
        {
            int retorno;
            AbreConexao(transacao);
            try
            {
                //Executa o comando de insert e retorna o @@IDENTITY
                retorno = Convert.ToInt32(Command.ExecuteScalar());
            }
            catch (Exception ex)
            {
                retorno = -1;
#if DEBUG
                throw new Exception("Erro ao executar o comando SQL: ", ex);
#endif
            }
            finally
            {
                if (!transacao)
                {
                    FechaConexao();
                }
            }
            return retorno;
        }

        /// <summary>
        /// Método responsável pela execução dos comandos Select
        /// </summary>
        /// <returns>Retorna um DataTable com o resultado da operação</returns>
        public DataTable ExecutaSelect(bool isTransacao = false)
        {
            AbreConexao(isTransacao);
            DataTable dt = new DataTable();
            try
            {
                dt.Load(Command.ExecuteReader());
            }
            catch (Exception)
            {
                dt = null;
            }
            finally
            {
                if (!isTransacao)
                    FechaConexao();
            }
            return dt;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
