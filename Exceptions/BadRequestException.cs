using AutoMapper;

namespace EzhikLoader.Server.Exceptions
{
    public class BadRequestException(string message) : Exception(message) { }
}
