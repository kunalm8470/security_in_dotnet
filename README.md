# Security techniques in .NET

## Table of Contents  
- [Cryptography 101](#cryptography-101)
    - [Hashing](#hashing)
    - [Symmetric Key Encryption](#symmetric-key-encryption)
    - [Asymmetric Key Encryption](#Asymmetric-key-encryption)
- [Basic Authentication](#basic-authentication)

## Cryptography 101
___

Cryptography in .NET can be done using hashing, encryption and decryption using symmetric and asymmetric key algorithms.

### **Hashing**

For hashing we can implement it using various algorithms like MD5, SHA1, SHA2 series, Bcrypt.

But MD5 and SHA1 are no longer considered safe now and best practice is to either use SHA-2 family (SHA256, SHA512, etc..) or Bcrypt.

This project implements hashing using [`SHA512`](./Hashing/Hashing/Models/ShaHasher.cs#L14-L25) and [`Bcrypt`](./Hashing/Hashing/Models/BcryptHasher.cs#L7-L13) using a common interface [`IHasher`](./Hashing/Hashing/Interfaces/IHasher.cs)

### **Symmetric Key Encryption**

For symmetric key encryption we use [`AES algorithm`](https://en.wikipedia.org/wiki/Advanced_Encryption_Standard) as its the industry standard for symmetric key encryption.

AES algorithm comes with 128, 256 bit key sizes. The bigger the key size is harder it is to brute force it.

We are using CBC mode in which the plain text is broken into blocks 
and encrypted. Each block is then encrypted separately with using IV for the first block and passing the next IV from the output of first block and so on.

[`SymmetricKeyEncryption`](./SymmetricKeyEncryption/SymmetricKeyEncryption/SymmetricKeyEncryption.cs) is a wrapper over .NET implementation of AES which then [**encrypts**](./SymmetricKeyEncryption/SymmetricKeyEncryption/SymmetricKeyEncryption.cs#L35-42) and [**decrypts**](./SymmetricKeyEncryption/SymmetricKeyEncryption/SymmetricKeyEncryption.cs#L26-33) respectively.

### **Asymmetric Key Encryption**

For Asymmetric key encryption we use [`RSA algorithm`](https://en.wikipedia.org/wiki/RSA_(cryptosystem)) as its the industry standard for symmetric key encryption.

RSA algorithm comes with 512, 1024, 2048, 3076 and 4096 bit key sizes. The bigger the key size is harder it is to brute force it.

We are using [`Pkcs1`](https://en.wikipedia.org/wiki/PKCS1) padding to prevent it against known [`vulnerabilities`](https://en.wikipedia.org/wiki/RSA_(cryptosystem)#Attacks_against_plain_RSA).

To generate a self-signed public-private key pair in PEM format, we use OpenSSL using GitBash -

Private key -
```shell
openssl genpkey -algorithm RSA -out private_key.pem -pkeyopt rsa_keygen_bits:4096
```

Public key -
```
openssl rsa -pubout -in private_key.pem -out public_key.pem
```

We then read the PEM files using new .NET 5 method [`ImportFromPem`](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.rsa.importfrompem) which expects a span of characters, for older .NET versions we can use the BouncyCastle library.

## Basic Authentication
___

Basic authentication expects us to pass the credentials in the authorization header in the format for every request.

```shell
Authorization: Basic base64(username:password)
```

If used under HTTPS its moderately secure, but if used in HTTP its very unsecure as we can intercept the request headers and attack.

If we don't provide the Authorization header then the server will return a [`WWW-authenticate header`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/WWW-Authenticate) back meaning us to authenticate.

In .NET core we can implement basic authentication by inheriting the [`AuthenticationHandler`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.authenticationhandler-1?view=aspnetcore-5.0) abstract class and [`overriding`](./BasicAuthentication/src/Api/Authentication/BasicAuthenticationHandler.cs#L39-L84) the [`HandleAuthenticateAsync`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.authenticationhandler-1.handleauthenticateasync?view=aspnetcore-5.0#Microsoft_AspNetCore_Authentication_AuthenticationHandler_1_HandleAuthenticateAsync) method.

Post doing that we should register it in the [`ConfigureServices`](./BasicAuthentication/src/Api/Startup.cs#L41-L43) method and adding the [`app.UseAuthentication()`](./BasicAuthentication/src/Api/Startup.cs#L57) middleware in the `Configure` method.

Schema for the user is -
```javascript
{
    "first_name": "John",
    "last_name": "Doe",
    "gender_abbreviation": "M",
    "date_of_birth": "1970-01-01T10:40:00",
    "phone": "1234567890",
    "username": "johndoe123",
    "email": "johndoe.123@email.com",
    "password": "1234567890",
    "compare_password": "1234567890"
}
```

Postman [`collection`](https://www.getpostman.com/collections/fdc42ec10f878e04c258) and [`environment variables`](./BasicAuthentication/Local.postman_environment.json) here.