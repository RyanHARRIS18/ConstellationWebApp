To start this app:

run build.sh 
or 
build.cmd

watch for the output: Successfully built 13faf875633a

docker run -p 8080:80 13faf875633a (alphanumeric string comes from the above output)

then go to http://localhost:8080 in the browser
