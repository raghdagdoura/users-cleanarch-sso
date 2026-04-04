
import { useState } from "react";
import "./App.css";

function App() {
const [user, setUser] = useState(null);


const handleLogin = async () => {
try {
const url = "https://congenial-space-train-5p6vjp596jvcvx97-5251.app.github.dev/api/Auth/login?email=admin@test.com";

console.log("CALL API:", url);

const response = await fetch(url, {
method: "POST",
});

console.log("STATUS:", response.status);

// 🔥 AJOUT IMPORTANT
if (!response.ok) {
const text = await response.text();
console.error("API ERROR:", text);
throw new Error("API error " + response.status);
}

const data = await response.json();

console.log("DATA:", data);

setUser({
name: "Raghda",
email: "admin@test.com",
role: data.role,
token: data.token,
});

} catch (error) {
console.error("Erreur login:", error);
alert("Erreur pendant le login. Regarde la console.");
}
};


const handleLogout = () => {
setUser(null);
};

return (
<div className="container">
<div className="card">
{!user ? (
<>
<h1>UsersApp</h1>
<p>Connexion avec Microsoft</p>

<button className="ms-button" onClick={handleLogin}>
🔐 Se connecter avec Microsoft
</button>
</>
) : (
<>
<h1>Bienvenue</h1>
<p><strong>Nom :</strong> {user.name}</p>
<p><strong>Email :</strong> {user.email}</p>
<p><strong>Rôle :</strong> {user.role}</p>

<button className="logout-button" onClick={handleLogout}>
Se déconnecter
</button>
</>
)}
</div>
</div>
);
}

export default App;
