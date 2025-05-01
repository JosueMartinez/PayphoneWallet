
# PayphoneWallet API

Este proyecto es una API REST para gestionar billeteras y realizar transferencias entre ellas. La API incluye operaciones CRUD para billeteras y operaciones de historial de movimientos. Además, cuenta con autenticación básica y manejo de errores adecuados.

## Requisitos

- .NET 8 SDK
- SQL Server o cualquier base de datos compatible con Entity Framework Core
- Visual Studio o cualquier editor de código compatible

## Pasos para levantar el proyecto localmente

### 1. Clonar el repositorio

Primero, clona este repositorio en tu máquina local:

```bash
git clone https://github.com/JosueMartinez/PayphoneWallet.git
cd PayphoneWallet
```

### 2. Restaurar los paquetes NuGet

Para asegurarte de que todos los paquetes NuGet estén instalados, ejecuta el siguiente comando en el directorio del proyecto:

```bash
dotnet restore
```

### 3. Configuración de la conexión a la base de datos

Antes de ejecutar el proyecto, necesitarás configurar la conexión a la base de datos en el archivo `appsettings.json`. Abre el archivo `PayphoneWallet/appsettings.json` y actualiza la sección de la base de datos con los parámetros correctos para tu servidor:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVIDOR;Database=PayphoneWalletDb;User Id=TU_USUARIO;Password=TU_CONTRASEÑA;"
}
```

Asegúrate de cambiar `TU_SERVIDOR`, `TU_USUARIO`, y `TU_CONTRASEÑA` por los valores correctos para tu servidor de base de datos.

### 4. Crear la base de datos

Para crear la base de datos localmente, ejecuta el siguiente comando:

```bash
dotnet ef database update --project PayphoneWallet.Infrastructure --startup-project PayphoneWallet
```

Este comando aplicará las migraciones y creará las tablas necesarias en tu base de datos.

### 5. Ejecutar el proyecto

Una vez que la base de datos está configurada, puedes ejecutar el proyecto con el siguiente comando:

```bash
dotnet run --project PayphoneWallet
```

### 6. Pruebas

Este proyecto incluye pruebas unitarias e integración. Para ejecutar las pruebas, puedes usar el siguiente comando:

```bash
dotnet test
```

### 7. Autenticación y Autorización

La API utiliza **autenticación básica** para proteger los endpoints que permiten realizar acciones sobre las billeteras y las transferencias. A continuación, se especifican los requisitos de autenticación para las operaciones:

#### Endpoints protegidos que requieren autenticación:
- **Crear billetera** (POST /api/wallets)
- **Actualizar billetera** (PUT /api/wallets)
- **Eliminar billetera** (DELETE /api/wallets/{id})
- **Realizar transferencia** (POST /api/transactions)

Los **usuarios no autenticados** solo podrán acceder al siguiente endpoint:
- **Listar historial de movimientos** (GET /api/transactions)

#### Obtener un Token de Autenticación

Para realizar solicitudes a los endpoints protegidos, primero necesitas obtener un token de autenticación. Puedes hacerlo utilizando el endpoint `/api/auth/login`, proporcionando tus credenciales de usuario (nombre de usuario y contraseña).

**Endpoint para obtener el token**:
```
POST /api/auth/login
```

**Cuerpo de la solicitud** (JSON):
```json
{
  "username": "tu_usuario",
  "password": "tu_contraseña"
}
```

**Respuesta (200 OK)**:
Si las credenciales son correctas, recibirás un token JWT que podrás usar para autenticarte en futuras solicitudes.

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6Ikp1YW4gRHVyYW4iLCJpYXQiOjE1MTYyMzkwMjJ9.kIWG9xLbO2TKR9M8YogGnoLjFryQkGZdcH8qXZ-6JlY"
}
```

#### Usar el Token en las Solicitudes

Una vez que tengas el token, debes incluirlo en el encabezado **Authorization** de cada solicitud que requiera autenticación.

**Ejemplo de solicitud con token**:
```http
POST /api/wallets
Authorization: Bearer <tu_token>
```

## Notas

- Asegúrate de que la conexión a la base de datos esté correctamente configurada antes de ejecutar la aplicación.

## Contribución

Si deseas contribuir, por favor realiza un fork del repositorio, crea una nueva rama y envía un pull request con tus cambios.
