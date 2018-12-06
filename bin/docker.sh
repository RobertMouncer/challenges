echo "$ENCRYPTED_DOCKER_PASSWORD" | docker login -u "$ENCRYPTED_DOCKER_USERNAME" --password-stdin
cd challenges
docker build -t sem56402018/challenges:$1 -t sem56402018/challenges:$TRAVIS_COMMIT .
docker push sem56402018/challenges:$TRAVIS_COMMIT
docker push sem56402018/challenges:$1