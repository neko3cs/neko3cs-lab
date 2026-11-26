FROM docker.io/library/redmine:7.0.3-bookworm

RUN git clone --depth 1 --branch v7.0.0 \
      https://github.com/momibun926/redmine_issue_evm.git plugins/redmine_issue_evm \
    && rm -rf plugins/redmine_issue_evm/.git \
    && printf 'production:\n  adapter: sqlite3\n  database: sqlite/redmine.db\n' > config/database.yml \
    && bundle install \
    && chown redmine:redmine Gemfile.lock config/database.yml

ENV REDMINE_PLUGINS_MIGRATE=1
USER redmine
CMD ["bundle", "exec", "sh", "-c", "bin/rails assets:precompile && exec rails server -b 0.0.0.0"]
