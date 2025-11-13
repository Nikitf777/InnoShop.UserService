namespace InnoShop.UserService.Exceptions;

public class NotFoundException(string message) : Exception(message);
public class UserExistsException(string message) : Exception(message);