<?php
$servername = "localhost";
$username = "root";
$password = "mysql";
$database = "training";

// Create connection
$conn = new mysqli($servername, $username, $password, $database);

// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
} 
echo "Connection successful<br>";

$firstname = $_POST["firstname"];
$lastname = $_POST["lastname"];
$email = $_POST["email"];
$test = $_POST["test"];
$duration = $_POST["duration"];
$timeOff = $_POST["timeOff"];
$cntOff = $_POST["cntOff"];
$sql = "INSERT INTO results (firstname, lastname, email, 
test, duration, timeOff, cntOff, timestamp)
VALUES ('".$firstname."','".$lastname."','".$email."','".
$test."',".$duration.",".$timeOff.",".$cntOff.",now())";

if ($conn->query($sql) === TRUE) {
    echo "Post successful<br>";
} else {
    echo "Error: " . $sql . "<br>" . $conn->error;
}

$conn->close();
?>