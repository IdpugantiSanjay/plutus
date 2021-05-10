#!/bin/sh
if [ "$(id -u)" = "0" ]; then
  # running on a developer laptop as root
  fix-perms -r -u app -g app /src
  exec gosu app "$@"
else
  # running in production as a user
  exec "$@"
fi