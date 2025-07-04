# 🎬 Projeto CineBR

CineBR é uma plataforma web desenvolvida com o framework ASP.NET Core MVC que tem como objetivo promover e facilitar o acesso a trailers e informações sobre **filmes nacionais brasileiros**. O sistema possui área administrativa, funcionalidades de curtir, comentar, buscar e visualizar trailers, além de um layout responsivo com foco na experiência do usuário.

# A plataforma está disponvel no seguinte link >>>[ Acessar CineBR](https://projetocinebr.onrender.com/)

---

## 📌 Funcionalidades Principais

### 👤 Área do Usuário
- Cadastro e login de usuários.
- Visualização de catálogo de filmes brasileiros.
- Curtir filmes favoritos.
- Comentar nos filmes.
- Página de detalhes com trailer e descrição.
- Página com lista dos filmes curtidos.

### 🛠️ Área do Administrador
- Cadastro de novos filmes (com título, descrição, gênero, trailer, etc).

### 📦 Dados
- Persistência feita através de arquivos `.json`:
  - `filmes.json`: informações dos filmes.
  - `usuarios.json`: dados de cadastro.
  - `comentarios.json`: comentários deixados pelos usuários.
  - `likes.json`: sistema de curtidas.
  - `classifications.json`: classificação indicativa.

---

## 🧱 Arquitetura do Projeto

O CineBR segue o padrão **MVC (Model-View-Controller)** com separação clara de responsabilidades:

### 📁 Controllers
Responsáveis pelo controle das rotas e lógica de interação entre Views e Models:
- `FilmesController`
- `FilmesCurtidosController`
- `ComentarioController`
- `UsuarioController`
- `EmailController`
- `AdminPagesController`
- `HomeController`

### 📁 Models
Modelos que representam as entidades principais da aplicação:
- `Filme.cs`
- `Usuario.cs`
- `Comentario.cs`
- `Like.cs`
- `Classification.cs`

### 📁 Services
Camada de serviço onde fica a lógica de manipulação dos dados `.json`:
- `FilmeService.cs`
- `UsuarioService.cs`
- `ComentarioService.cs`
- `LikeService.cs`
- `EmailService.cs`

### 📁 Views
Interface visual feita com **Razor Pages**:
- `Index`, `Detalhes`, `CadastroLogin`, `DashboardFilmes`, `CadastroFilmes`, etc.
- Layouts separados para usuários comuns (`_Layout.cshtml`) e administradores (`_LayoutAdmin.cshtml`).

---

## 🌐 Front-end

### Tecnologias Utilizadas
- HTML5 / CSS3
- JavaScript (puro)
- [Swiper.js](https://swiperjs.com/) (para sliders de filmes)
- [Bootstrap](https://getbootstrap.com/) (layout responsivo)

### Scripts Personalizados
- `catalogo.js`: carrega e organiza os filmes do catálogo.
- `categoria.js`, `diretor-elenco.js`, `main.js`, `search-box.js`: funcionalidades específicas.
- `swiper-bundle.min.js`: carrossel de filmes.

---

## 🛠️ Back-end

### Tecnologias
- ASP.NET Core 6.0
- C# (linguagem principal)
- Serviços internos para leitura e escrita em arquivos `.json`
- Validação de dados no back-end
- Serviço de envio de e-mails (`EmailService.cs`)

---

## 🐳 Docker

O projeto contém um `Dockerfile`, permitindo que seja containerizado para facilitar o deploy em ambientes como Render, AWS, Azure, ou máquinas locais com Docker:

```dockerfile
# Dockerfile básico presente no projeto
