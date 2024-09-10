using System.Data;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Data.SqlClient;
using WEB.DTOs;
using WEB.Interface.IRepository;
using WEB.Models;

namespace WEB.Interface.Repository;

public class CheckoutRepository : ICheckoutRepository
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public CheckoutRepository(IConfiguration configuration)
    {
        this._configuration = configuration;
        _connectionString = _configuration.GetConnectionString("MyDbCon");
    }

    public async Task<List<Checkout>> GetCheckout()
    {
        var checkouts = new List<Checkout>();
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var command = new SqlCommand("selectCheckoutWeb", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            DataTable dtable = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dtable);

            foreach (DataRow row in dtable.Rows)
            {
                var checkoutObj = new Checkout()
                {
                    Id = (Guid)row["Id"],
                    Name = (string)row["Name"],
                    Amount = (int)row["Amount"],
                    Remarks = (string)row["Remarks"],
                    Status = (string)row["Status"]
                };
                checkouts.Add(checkoutObj);
            }
            await connection.CloseAsync();

            return checkouts;
        }
    }

    public async Task AddCheckout(Checkout checkoutModel)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var command = new SqlCommand("insertCheckoutWeb", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Id", checkoutModel.Id);
            command.Parameters.AddWithValue("@Name", checkoutModel.Name);
            command.Parameters.AddWithValue("@Amount", checkoutModel.Amount);
            command.Parameters.AddWithValue("@Remarks", checkoutModel.Remarks);
            command.Parameters.AddWithValue("@Status", checkoutModel.Status);

            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task<Checkout> ProcessCheckout(CheckoutDTO checkoutDto)
    {
        var newId = Guid.NewGuid();

        var check = new Checkout()
        {
            Id = newId,
            Name = checkoutDto.Name,
            Amount = checkoutDto.Amount,
            Remarks = checkoutDto.Remarks,
        };

        int num = GenerateStatus();
        check.Status = num <= 3 ? "Success" : "Failed";

        await AddCheckout(check);
        return check;
    }

    public async Task<Checkout> GetCheckoutById(Guid id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var command = new SqlCommand("selectCheckoutById", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };
            command.Parameters.AddWithValue("@Id", id);

            DataTable dtable = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dtable);

            if (dtable.Rows.Count > 0)
            {
                Checkout checkoutObj = new Checkout()
                {
                    Id = (Guid)dtable.Rows[0]["Id"],
                    Name = (string)dtable.Rows[0]["Name"],
                    Amount = (int)dtable.Rows[0]["Amount"],
                    Remarks = (string)dtable.Rows[0]["Remarks"],
                    Status = (string)dtable.Rows[0]["Status"]
                };
                await connection.CloseAsync();
                return checkoutObj;
            }

            return null;
        }
    }

    private int GenerateStatus()
    {
        Random random = new Random();
        return random.Next(1, 7); // 1 to 6 samma auxa 
    }

    public async Task SoftDeleteCheckout(Guid id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var command = new SqlCommand("updateCheckoutWebIsDeleted", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();

            await connection.CloseAsync();
        }
    }
}