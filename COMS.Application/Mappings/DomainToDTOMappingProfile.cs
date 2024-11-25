using AutoMapper;
using COMS.Application.DTOs.Customer;
using COMS.Application.DTOs.Order;
using COMS.Application.DTOs.OrderItem;
using COMS.Application.DTOs.Product;
using COMS.Application.DTOs.Transaction;
using COMS.Domain.Entities;
using System.Security.Cryptography;

namespace COMS.Application.Mappings
{
    public class DomainToDTOMappingProfile : Profile
    {
        private readonly string EncryptionKey = "YaOI3HmanVzi/QVCiFpjX2Z3XOMtZp5VClq3tvJUngY=";

        public DomainToDTOMappingProfile()
        {
            #region :: Customer
            CreateMap<Customer, CustomerDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom((src, dest) => Decrypt(src.Name, src.EncryptionIV)))
                .ForMember(dest => dest.Email, opt => opt.MapFrom((src, dest) => Decrypt(src.Email, src.EncryptionIV)))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom((src, dest) => Decrypt(src.Phone, src.EncryptionIV)))
                .ReverseMap()
                .ForMember(dest => dest.Name, opt => opt.MapFrom((src, dest) => Encrypt(src.Name, dest.EncryptionIV)))
                .ForMember(dest => dest.Email, opt => opt.MapFrom((src, dest) => Encrypt(src.Email, dest.EncryptionIV)))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom((src, dest) => Encrypt(src.Phone, dest.EncryptionIV)));

            CreateMap<Customer, CustomerDetailedDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom((src, dest) => Decrypt(src.Name, src.EncryptionIV)))
                .ForMember(dest => dest.Email, opt => opt.MapFrom((src, dest) => Decrypt(src.Email, src.EncryptionIV)))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom((src, dest) => Decrypt(src.Phone, src.EncryptionIV)))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom((src, dest) => Encrypt(src.Name, dest.EncryptionIV)))
                .ForMember(dest => dest.Email, opt => opt.MapFrom((src, dest) => Encrypt(src.Email, dest.EncryptionIV)))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom((src, dest) => Encrypt(src.Phone, dest.EncryptionIV)));
            #endregion

            #region :: Product
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, ProductDetailedDTO>().ReverseMap();
            #endregion

            #region :: Order
            CreateMap<Order, OrderDTO>().ReverseMap();

            CreateMap<Order, OrderDetailedDTO>()
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom((src, dest) => src.OrderItems.Sum(oi => oi.Quantity * oi.Product.Price)))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ReverseMap();
            #endregion

            #region :: OrderItem
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();

            CreateMap<OrderItem, OrderItemDetailedDTO>()
                .ForMember(dest => dest.ItemTotal, opt => opt.MapFrom(src => src.Quantity * src.Product.Price));
            #endregion

            #region :: Transaction
            CreateMap<Transaction, TransactionDTO>()
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TransactionId));

            CreateMap<Transaction, TransactionDetailedDTO>().ReverseMap();
            #endregion
        }

        #region :: Funções de encriptação e decriptação
        private string Decrypt(string encryptedField, string encryptionIV)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Convert.FromBase64String(EncryptionKey);
                aesAlg.IV = Convert.FromBase64String(encryptionIV);

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedField)))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd(); // Retorna o valor descriptografado
                }
            }
        }

        private string Encrypt(string plainText, string encryptionIV)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Convert.FromBase64String(EncryptionKey);
                aesAlg.IV = Convert.FromBase64String(encryptionIV);

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                    swEncrypt.Close();
                    return Convert.ToBase64String(msEncrypt.ToArray()); // Retorna o valor encriptografado
                }
            }
        }
        #endregion
    }
}
