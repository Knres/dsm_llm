# dsm_llm

## Cambios Recientes

### Mejoras en la Testabilidad (3 de noviembre de 2025)

- **Clases EN (Entidades)**: Se añadieron inicializadores `= null!;` a las propiedades de referencia no anulables para eliminar las advertencias del compilador sin cambiar el comportamiento en runtime.
  - Clases afectadas: `Usuario`, `Resena`, `Pelicula`, `Reporte`, `Notificacion`, `Metrica`, `Lista`, `Administrador`

- **Clases CEN (Lógica de Negocio)**: Se marcaron los métodos públicos como `virtual` para permitir el mocking en pruebas unitarias.
  - Clases afectadas: `PeliculaCEN`, `UsuarioCEN`

- **Clases CP (Control Process)**: Se marcaron los métodos públicos como `virtual` para facilitar el mocking en pruebas unitarias.
  - Clases afectadas: `ManageUsuariosCP`

Estos cambios mejoran la capacidad de prueba del código sin afectar su funcionalidad, permitiendo un mejor testing mediante mocking con frameworks como Moq.