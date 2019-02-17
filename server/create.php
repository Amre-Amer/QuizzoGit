<?php
$servername = "localhost";
$username = "root";
$password = "mysql";
$database = "training";

// Create connection
$conn = new mysqli($servername, $username, $password);
//$conn = mysql_connect($servername, $username, $password);

// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
} 
echo "Connection successful<br>";

$sql = "DROP DATABASE ".$database;
if ($conn->query($sql) === TRUE) {
    echo "DROP DATABASE successful<br>";
} else {
    echo "Error dropping database: " . $conn->error."<br>";
}

$sql = "CREATE DATABASE ".$database;
if ($conn->query($sql) === TRUE) {
    echo "CREATE DATABASE successful<br>";
} else {
    echo "Error creating database: " . $conn->error."<br>";
}

$db_selected = mysqli_select_db($conn, $database);
if ($db_selected) {
    echo "SELECT DATABASE successful<br>";
} else {
    die ('Error selecting database : ' . mysql_error());
}

$sql = "CREATE TABLE results (
id INT(6) UNSIGNED AUTO_INCREMENT PRIMARY KEY,
firstname VARCHAR(30) NOT NULL,
lastname VARCHAR(30) NOT NULL,
email VARCHAR(50),
test VARCHAR(50),
duration FLOAT(8,4),
timeOff FLOAT(8,4),
cntOff INT(6),
timestamp TIMESTAMP
)";
if ($conn->query($sql) === TRUE) {
    echo "CREATE TABLE successful<br>";
} else {
    echo "Error creating table: " . $conn->error."<br>";
}

$conn->close();
?>