class ApplicationMailbox < ActionMailbox::Base
  routing /\Acomment\+[0-9]+/i => :comment
end
