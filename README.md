
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

El servidor debería estar disponible en `http://localhost:5000` por defecto.

### 6. Pruebas

Este proyecto incluye pruebas unitarias e integración. Para ejecutar las pruebas, puedes usar el siguiente comando:

```bash
dotnet test
```

### 7. Autenticación

La API utiliza autenticación básica para acceder a los endpoints de creación, actualización y eliminación de billeteras, así como las transferencias. Los usuarios no autenticados solo podrán acceder al endpoint de listar el historial de movimientos.

Puedes usar un token de autenticación (por ejemplo, en los headers) al realizar solicitudes a los endpoints que requieren autenticación.

## Endpoints

### 1. **GET /api/wallets**

Obtiene la lista de todas las billeteras.

### 2. **POST /api/wallets**

Crea una nueva billetera.

### 3. **GET /api/wallets/{id}**

Obtiene una billetera por su ID.

### 4. **PUT /api/wallets/{id}**

Actualiza una billetera existente.

### 5. **DELETE /api/wallets/{id}**

Elimina una billetera.

### 6. **POST /api/transactions**

Realiza una nueva transferencia entre billeteras.

### 7. **GET /api/transactions**

Obtiene el historial de transacciones.

## Notas

- Asegúrate de que la conexión a la base de datos esté correctamente configurada antes de ejecutar la aplicación.

## Contribución

Si deseas contribuir, por favor realiza un fork del repositorio, crea una nueva rama y envía un pull request con tus cambios.
