using GestoraSample.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestoraSample.Infra.Dados.Old
{
    public class clienteRepo : Contexto
    {
        public bool Inserir(Cliente cliente)
        {
            Command = new System.Data.SqlClient.SqlCommand();
            Command.CommandType = System.Data.CommandType.Text;
            Command.CommandText = @"INSERT INTO CLIENTE 
                                    (NOME, CPF, TELEFONE) 
                                    VALUES 
                                    (@NOME, @CPF, @TELEFONE)";

            Command.Parameters.Add("@NOME", System.Data.SqlDbType.VarChar).Value = cliente.Nome;
            Command.Parameters.Add("@CPF", System.Data.SqlDbType.VarChar).Value = cliente.Cpf;
            Command.Parameters.Add("@TELEFONE", System.Data.SqlDbType.VarChar).Value = cliente.Telefone;

            if (ExecutaComando(false) == 1)
                return true;

            return false;
        }

        public bool Alterar(Cliente cliente)
        {
            Command = new System.Data.SqlClient.SqlCommand();
            Command.CommandType = System.Data.CommandType.Text;

            Command.CommandText = @"UPDATE 
                                    CLIENTE SET NOME = @NOME, 
                                    CPF = @CPF, 
                                    TELEFONE = @TELEFONE 
                                    WHERE 
                                    CLIENTEID = @CLIENTEID";

            Command.Parameters.Add("@NOME", System.Data.SqlDbType.VarChar).Value = cliente.Nome;
            Command.Parameters.Add("@CPF", System.Data.SqlDbType.VarChar).Value = cliente.Cpf;
            Command.Parameters.Add("@TELEFONE", System.Data.SqlDbType.VarChar).Value = cliente.Telefone;
            Command.Parameters.Add("@CLIENTEID", System.Data.SqlDbType.BigInt).Value = cliente.ClienteId;

            if (ExecutaComando(false) == 1)
                return true;

            return false;
        }

        public bool Excluir(Guid clienteId)
        {
            Command = new System.Data.SqlClient.SqlCommand();
            Command.CommandType = System.Data.CommandType.Text;

            Command.CommandText = @"DELETE CLIENTE
                                    WHERE 
                                    CLIENTEID = @CLIENTEID";

            Command.Parameters.Add("@CLIENTEID", System.Data.SqlDbType.BigInt).Value = clienteId;

            if (ExecutaComando(false) == 1)
                return true;

            return false;
        }

        public List<Cliente> SelecionarTodos()
        {
            Command = new System.Data.SqlClient.SqlCommand();
            Command.CommandType = System.Data.CommandType.Text;
            Command.CommandText = @"SELECT * FROM CLIENTE";
            System.Data.DataTable dt = ExecutaSelect();

            return converteDataTable(dt);
        }

        public Cliente SelecionarPorId(Guid clienteId)
        {
            Command = new System.Data.SqlClient.SqlCommand();
            Command.CommandType = System.Data.CommandType.Text;
            Command.CommandText = @"SELECT * FROM CLIENTE WHERE CLIENTEID = @CLIENTEID";
            Command.Parameters.Add("@CLIENTEID", System.Data.SqlDbType.BigInt).Value = clienteId;
            System.Data.DataTable dt = ExecutaSelect();

            return converteDataTable(dt).FirstOrDefault();
        }

        private List<Cliente> converteDataTable(System.Data.DataTable dt)
        {
            List<Cliente> list = new List<Cliente>();

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Guid clienteId = new Guid();
                    string nome = "";
                    string cpf = "";
                    string telefone = "";

                    if (dt.Rows[i]["ClienteId"] != null)
                    {
                        clienteId = Guid.Parse(Convert.ToString(dt.Rows[i]["ClienteId"]));
                    }

                    if (dt.Rows[i]["Nome"] != null)
                    {
                        nome = Convert.ToString(dt.Rows[i]["Nome"]);
                    }

                    if (dt.Rows[i]["Cpf"] != null)
                    {
                        cpf = Convert.ToString(dt.Rows[i]["Cpf"]);
                    }

                    if (dt.Rows[i]["Telefone"] != null)
                    {
                        telefone = Convert.ToString(dt.Rows[i]["Telefone"]);
                    }

                    list.Add(
                        new Cliente
                        {
                            ClienteId = clienteId,
                            Nome = nome,
                            Cpf = cpf,
                            Telefone = telefone
                        });
                }
            }

            return list;
        }
    }
}
