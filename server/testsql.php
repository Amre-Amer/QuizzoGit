<?php
$servername = "localhost";
$username = "root";
$password = "mysql";
$database = "courses";

// Create connection
$conn = new mysqli($servername, $username, $password, $database);

// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
} 
echo "Connected successfully";

// sql to create table
//$sql = "CREATE TABLE testers (
//id INT(6) UNSIGNED AUTO_INCREMENT PRIMARY KEY, 
//firstname VARCHAR(30) NOT NULL,
//lastname VARCHAR(30) NOT NULL,
//email VARCHAR(50),
//timestamp TIMESTAMP
//)";

//if ($conn->query($sql) === TRUE) {
//    echo "Table testers created successfully";
//} else {
//    echo "Error creating table: " . $conn->error;
//}

//    echo "POST: ";
//    print_r( $_POST );
//    var_dump( $_POST );

//    echo "GET: ";
//    print_r( $_GET );
//    var_dump( $_GET );

     $firstname = $_POST["firstname"];
     $lastname = $_POST["lastname"];
echo "post firstname ".$firstname." lastname ".$lastname;

$sql = "INSERT INTO testers (firstname, lastname, email, timestamp)
VALUES ('".$firstname."','".$lastname."','post.com', now())";

if ($conn->query($sql) === TRUE) {
    echo "New record created successfully";
} else {
    echo "Error: " . $sql . "<br>" . $conn->error;
}

$sql = "SELECT id, firstname, lastname, email, timestamp FROM testers";
$result = $conn->query($sql);

if ($result->num_rows > 0) {
    // output data of each row
    while($row = $result->fetch_assoc()) {
        echo "id: " . $row["id"]. " - Name: " . $row["firstname"]. " " . $row["lastname"]. " " . $row["email"]. " " . $row["timestamp"]. "<br>";
    }
} else {
    echo "0 results";
}

$sql = "SELECT name, description FROM courses";
$result = $conn->query($sql);

if ($result->num_rows > 0) {
    // output data of each row
    while($row = $result->fetch_assoc()) {
        echo "id: " . $row["id"]. " - Course: " . $row["name"]. " " . $row["description"]. "<br>";
    }
} else {
    echo "0 results";
}
$conn->close();
?>