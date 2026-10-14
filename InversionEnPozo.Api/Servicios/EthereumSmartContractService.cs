using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.RPC.Eth.DTOs;
using Microsoft.Extensions.Configuration;
using InversionEnPozo.Api.Servicios.Interfaces;

namespace InversionEnPozo.Api.Servicios
{
    public class EthereumSmartContractService : IEthereumSmartContractService
    {
        private readonly string _rpcUrl;
        private readonly string _privateKey;

        public EthereumSmartContractService(IConfiguration configuration)
        {
            _rpcUrl = configuration["Ethereum:RpcUrl"] ?? "https://sepolia.infura.io/v3/0da7002e505941068621e494a11517a2";
            _privateKey = configuration["Ethereum:PrivateKey"] ?? throw new InvalidOperationException("Falta configurar la PrivateKey de Ethereum.");
        }

        public async Task<string> RegistrarProyectoConContratoAsync(string nombreProyecto, decimal presupuesto)
        {
            try
            {
                var account = new Account(_privateKey);
                var web3 = new Web3(account, _rpcUrl);

                var senderAddress = account.Address;
                var gasPrice = await web3.Eth.GasPrice.SendRequestAsync();

                var transferRequest = new TransactionInput()
                {
                    From = senderAddress,
                    To = senderAddress,
                    Value = new Nethereum.Hex.HexTypes.HexBigInteger(0),
                    Gas = new Nethereum.Hex.HexTypes.HexBigInteger(100000),
                    GasPrice = gasPrice
                };

                var transactionHash = await web3.Eth.TransactionManager.SendTransactionAsync(transferRequest);

                return transactionHash;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al interactuar con la red Sepolia: {ex.Message}");
            }
        }

        public async Task<(bool Exito, string Mensaje, string HashBloque)> AprobarHitoAsync(int hitoId)
        {
            try
            {
                var account = new Account(_privateKey);
                var web3 = new Web3(account, _rpcUrl);

                var senderAddress = account.Address;
                var gasPrice = await web3.Eth.GasPrice.SendRequestAsync();

                var hitoRequest = new TransactionInput()
                {
                    From = senderAddress,
                    To = senderAddress,
                    Value = new Nethereum.Hex.HexTypes.HexBigInteger(0),
                    Gas = new Nethereum.Hex.HexTypes.HexBigInteger(100000),
                    GasPrice = gasPrice
                };

                string txHashReal = await web3.Eth.TransactionManager.SendTransactionAsync(hitoRequest);

                return (true, "Hito aprobado y transacción procesada en la Blockchain Sepolia.", txHashReal);
            }
            catch (Exception ex)
            {
                return (false, $"Error en la red Ethereum: {ex.Message}", string.Empty);
            }
        }
    }
}